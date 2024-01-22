using OutsourcePlatformApp.Dto;
using OutsourcePlatformApp.Dto.order;
using OutsourcePlatformApp.Models;
using OutsourcePlatformApp.Repository;

namespace OutsourcePlatformApp.Service;

public class OrderService
{
    private readonly IOrderRepository orderRepository;
    private readonly ICompetenciesRepository competenciesRepository;
    private readonly IResponseRepository responseRepository;
    private readonly IUserRepository userRepository;
    private readonly IReviewRepository reviewRepository;
    private readonly IOrderRoleRepository orderRoleRepository;  
    private readonly INotificationRepository notificationRepository;

    public OrderService(IOrderRepository orderRepository, ICompetenciesRepository competenciesRepository,
        IUserRepository userRepository, IReviewRepository reviewRepository, IResponseRepository responseRepository,
        IOrderRoleRepository orderRoleRepository, INotificationRepository notificationRepository)
    {
        this.orderRepository = orderRepository;
        this.competenciesRepository = competenciesRepository;
        this.userRepository = userRepository;
        this.reviewRepository = reviewRepository;
        this.responseRepository = responseRepository;
        this.orderRoleRepository = orderRoleRepository;
        this.notificationRepository = notificationRepository;
    }

    public async Task<Order> CreateOrder(User user, OrderCreationDto creationDto)
    {
        var skills = new List<Skill>(creationDto.OrderSkills.Select(x => x.Name).Count());
        foreach (var skill in creationDto.OrderSkills.Select(x => x.Name))
            skills.Add(await competenciesRepository.GetSkillByNameAsync(skill));

        var categories = new List<Category>(creationDto.OrderCategories!.Count);
        foreach (var category in creationDto.OrderCategories)
            categories.Add(await competenciesRepository.GetCategoryByNameAsync(category.Name));

        var orderRole =
            await orderRoleRepository.GetOrderRolesByNames(creationDto.OrderVacancies.Select(x => x.OrderRole.Name)
                .ToList());

        var order = new Order
        {
            Price = creationDto.Price,
            Name = creationDto.Name,
            Description = creationDto.Description,
            Deadline = creationDto.Deadline,
            CompanyName = user.Executor != null ? $"{user.Surname} {user.Name} {user.SecondName}" : user.Customer.CompanyName,
            Creator = user,
            OrderVacancies = creationDto.OrderVacancies.Select(dto => new OrderVacancy
            {
                OrderRole = orderRole.First(x => x.Name.Equals(dto.OrderRole.Name)),
                MaxWorkers = dto.MaxWorkers,
            }).ToList(),
            OrderCategories = categories,
            OrderSkills = skills,
            IsPublished = true,
            IsCompleted = false,
        };
        return await orderRepository.CreateOrderAsync(order);
    }

    public async Task PublishOrder(User user, int orderId)
    {
        var order = await orderRepository.GetOrderByIdAsync(orderId);

        if (!user.Orders.Contains(order))
            throw new ArgumentException("Заказ не принадлежит пользователю");
        order.IsPublished = true;

        await orderRepository.SaveChangesAsync(order);
    }

    public async Task DeleteOrder(User user, int orderId)
    {
        var order = await orderRepository.GetOrderByIdAsync(orderId);

        if (!user.Orders.Contains(order))
            throw new ArgumentException("Заказ не принадлежит пользователю");

        if (order.OrderVacancies.SelectMany(x => x.Responses).Any(response => response.IsAccept == true) ||
            order.IsCompleted)
            throw new ArgumentException("Невозможно удалить, заказ уже приняли");

        await orderRepository.DeleteOrderAsync(order);
    }

    public async Task CreateResponse(Executor executor, int orderId, int vacancyId)
    {
        var order = await orderRepository.GetOrderByIdAsync(orderId);

        if (order.IsCompleted)
            throw new ArgumentException("Невозможно откликнуться, набор исполнителей завершен");
        if (order.Creator.Id == executor.UserId)
            throw new ArgumentException("Вы не можете откликнутся на свой проект");
        await orderRepository.CreateResponseAsync(executor, order, vacancyId);
    }

    public async Task UpdateResponse(bool isAccept, int vacancyId, int responseId)
    {
        var response = await orderRepository.GetResponseById(responseId);
        var order = await orderRepository.GetOrderByIdAsync(response.OrderVacancy.OrderId);
        var vacancy = order.OrderVacancies.FirstOrDefault(vacancy => vacancy.Id == vacancyId);

        if (order.IsCompleted ||
            (vacancy.MaxWorkers > 0 && vacancy.MaxWorkers == order.OrderVacancies.Where(x => x.Id == vacancyId)
                .SelectMany(vacancy => vacancy.Responses)
                .Count(r => r.IsAccept == true)))
            throw new ArgumentException("Невозможно принять исполнителя, набор исполнителей завершен");
        if (!isAccept)
            await responseRepository.RemoveResponseAsync(response);
        else
        {
            response.IsAccept = isAccept;
            await orderRepository.UpdateResponseAsync(response);
        }
    }

    public async Task<List<CustomerCommonOrderDto>> GetUserOrders(User user, int offset, int limit)
    {
        var orders = await orderRepository
            .GetUserOrdersAsync(user, offset, limit);
        return orders.Select(order => new CustomerCommonOrderDto(order.Id, order.Price, order.Name,
            order.Description,
            order.CompanyName,
            order.OrderVacancies.SelectMany(vacancy => vacancy.Responses).Count(r => r.IsAccept == null),
            order.Deadline,
            false,
            order.OrderVacancies.Select(x => new OrderVacancyDto
            {
                MaxWorkers = x.MaxWorkers,
                OrderRole = new OrderRoleDto { Id = x.OrderRole.Id, Name = x.OrderRole.Name }
            }), new OrderSkillsDto(order), new OrderCategoriesDto(order), order.IsCompleted,
            order.OrderVacancies.SelectMany(vacancy => vacancy.Responses).Count(r => r.IsAccept == true))).ToList();
    }

    public async Task<List<CommonOrderDto>> GetExecutorOrders(User user, int offset, int limit)
    {
        var orders = await orderRepository
            .GetExecutorOrdersAsync(user.Executor!, offset, limit);
        return orders.Select(order => new CommonOrderDto(order.Id, order.Price, order.Name, order.Description,
            order.CompanyName,
            order.OrderVacancies?.SelectMany(vacancy => vacancy.Responses).Count(r => r.IsAccept == null),
            order.Deadline,
            true,
            order.OrderVacancies?.SelectMany(vacancy => vacancy.Responses)
                .FirstOrDefault(response => response.ExecutorId == user.Executor.Id).IsCompleted ?? false,
            order.OrderVacancies.Select(x => new OrderVacancyDto
            {
                MaxWorkers = x.MaxWorkers,
                OrderRole = new OrderRoleDto { Id = x.OrderRole.Id, Name = x.OrderRole.Name }
            }), new OrderSkillsDto(order), new OrderCategoriesDto(order))).ToList();
    }

    public async Task<DetailOrderDto> GetDetailOrderInfo(User creator, int orderId)
    {
        var order = await orderRepository.GetOrderByIdAsync(orderId);

        var result = new DetailOrderDto(order, creator);
        var responses = order.OrderVacancies.SelectMany(vacancy => vacancy.Responses)?.ToList();

        var ids = (responses ?? new List<Response>(0)).Where(response => response.IsAccept != false)
            .Select(response => response.ExecutorId).ToList();

        var executors = new List<ExecutorResponseDto>(ids.Count);

        foreach (var executorId in ids)
        {
            var user = await userRepository.GetUserByExecutorIdAsync(executorId);
            var reviews = await reviewRepository.GetReviewsByUserIdAsync(user.Id);

            executors.Add(new ExecutorResponseDto(user, order, reviews));
        }

        result.ExecutorResponse = executors;
        return result;
    }

    public async Task<List<Order>> GetOrdersWithChosenFiltersAsync(
        int offset, int limit,
        List<SkillDto>? skillDtos = null,
        List<CategoryDto>? categoryDtos = null)
    {
        var dbOrders = await orderRepository.GetActualOrdersAsync();
        var dbSkills = dbOrders.SelectMany(order => order.OrderSkills).ToList();
        var dbCategories = dbOrders.SelectMany(order => order.OrderCategories!).ToList();

        if (skillDtos == null)
        {
            var intersectedCategoryNames = dbCategories
                .Select(x => x.Name)
                .Intersect(categoryDtos!.Select(x => x.Name));
            var orderCategories = dbCategories
                .Where(x => intersectedCategoryNames.Contains(x.Name))
                .SelectMany(c => c.Orders);

            return orderCategories
                .Distinct()
                .OrderBy(order => order.Deadline)
                .Skip(offset)
                .Take(limit)
                .ToList();
        }

        if (categoryDtos == null)
        {
            var intersectedSkillNames = dbSkills
                .Select(x => x.Name)
                .Intersect(skillDtos.Select(x => x.Name));

            var orderSkills = dbSkills
                .Where(x => intersectedSkillNames.Contains(x.Name))
                .SelectMany(x => x.Orders);

            return orderSkills
                .Distinct()
                .OrderBy(order => order.Deadline)
                .Skip(offset)
                .Take(limit)
                .ToList();
        }

        var intersectSkillNames = skillDtos
            .Select(dto => dto.Name)
            .Intersect(dbSkills.Select(x => x.Name));
        var intersectCategoryNames = categoryDtos
            .Select(x => x.Name)
            .Intersect(dbCategories.Select(x => x.Name));

        var skills = dbSkills
            .Where(x => intersectSkillNames.Contains(x.Name))
            .SelectMany(s => s.Orders).Distinct().ToList();

        var categories = dbCategories
            .Where(x => intersectCategoryNames.Contains(x.Name))
            .SelectMany(c => c.Orders).Distinct().ToList();

        return skills.Union(categories)
            .Distinct()
            .OrderBy(order => order.Deadline)
            .Skip(offset)
            .Take(limit)
            .ToList();
    }

    public async Task<OrderListResponse> GetOrderList(User? user, int offset, int limit,
        List<SkillDto>? skillDtos = null, List<CategoryDto>? categoryDtos = null)
    {
        var orders = new List<Order>();
        if (skillDtos == null && categoryDtos == null)
        {
            orders = await orderRepository.GetOrdersListAsync(offset, limit);
        }
        else
        {
            orders = await GetOrdersWithChosenFiltersAsync(offset, limit, skillDtos, categoryDtos);
        }

        var count = await orderRepository.GetTotalOrderCountAsync();
        var executor = user?.Executor ?? null;

        return new OrderListResponse(orders.Select(order =>
        {
            var isResponsed = false;
            var isCompleted = false;
            if (executor != null)
            {
                var response = responseRepository.GetUserOrderResponseAsync(user.Executor.Id, order.Id)
                    .Result;
                isResponsed = response != null;
                if (response != null)
                    isCompleted = response.IsCompleted;
            }

            return new CommonOrderDto(
                order.Id,
                order.Price,
                order.Name,
                order.Description,
                order.CompanyName,
                order.OrderVacancies.SelectMany(x => x.Responses).Count(),
                order.Deadline,
                isResponsed,
                isCompleted,
                order.OrderVacancies.Select(x => new OrderVacancyDto
                {
                    MaxWorkers = x.MaxWorkers,
                    OrderRole = new OrderRoleDto { Id = x.OrderRole.Id, Name = x.OrderRole.Name }
                }),
                new OrderSkillsDto(order),
                new OrderCategoriesDto(order));
        }).ToList(), count);
    }

    public async Task<CommonOrderInfoDto> GetCommonOrderInfo(User? user, int orderId)
    {
        var order = await orderRepository.GetOrderByIdAsync(orderId);
        var isResponsed = false;
        var isAccepted = false;
        var isCompleted = false;
        Response? response = null;
        if (user != null && user.Executor != null)
        {
            response = order.OrderVacancies.SelectMany(x => x.Responses).FirstOrDefault(response =>
                response.ExecutorId == user.Executor.Id);

            isResponsed = response != null;
            isAccepted = response?.IsAccept ?? false;
            isCompleted = response?.IsCompleted ?? false;
        }

        var customer = await userRepository.GetUserByIdAsync(order.Creator.Id);
        return new CommonOrderInfoDto
        {
            OrderId = orderId,
            Price = order.Price,
            Name = order.Name,
            Description = order.Description,
            CompanyName = order.CompanyName,
            Deadline = order.Deadline,
            IsAccepted = response != null && response.ExecutorId == user.Executor.Id ? isAccepted : false,
            IsResponded = response != null && response.ExecutorId == user.Executor.Id ? isResponsed : false,
            IsCompleted = response != null && response.ExecutorId == user.Executor.Id ? isCompleted : false,
            OrderCategories = order.OrderCategories.Select(category => new CategoryDto
            {
                Id = category.Id,
                Name = category.Name
            }).ToList(),
            OrderSkills = order.OrderSkills.Select(category => new SkillDto
            {
                Id = category.Id,
                Name = category.Name
            }).ToList(),
            OrderVacancies = order.OrderVacancies.Select(x => new OrderVacancyDto
            {
                MaxWorkers = x.MaxWorkers,
                Id = x.Id,
                Responses = x.Responses?.Select(x => new ExecutorResponseDto()).ToList(),
                OrderRole = new OrderRoleDto { Name = x.OrderRole.Name, Id = x.OrderRole.Id }
            }).ToList(),
            Nickname = customer.Username,
            Address = customer.Customer == null ? string.Empty : customer.Customer.Address,
            INN = customer.Customer == null ? string.Empty : customer.Customer.INN,
            Email = customer.Email,
            Phone = customer.Phone,
            Contacts = new ContactsUpdateDto
            {
                Contacts = customer.UserContacts == null
                    ? null
                    : customer?.UserContacts?.ContactLinks?.Select(x => new ContactDto(x.Name, x.Url))?.ToList()
            }
        };
    }

    public async Task<Review> CreateReview(ReviewCreationDto reviewCreationDto)
    {
        var executor = await userRepository.GetUserByExecutorIdAsync(reviewCreationDto.ExecutorId);
        var order = await orderRepository.GetOrderByIdAsync(reviewCreationDto.OrderId);
        var response = order.OrderVacancies.SelectMany(x => x.Responses)
            .FirstOrDefault(r => r.ExecutorId == reviewCreationDto.ExecutorId);
        if (response == null)
            throw new ArgumentException("");

        if (!response.IsCompleted && order.Deadline <= DateOnly.FromDateTime(DateTime.Now))
            throw new ArgumentException("Заказ не закрыт");

        if (await reviewRepository.IsReviewExists(order, executor))
            throw new ArgumentException("Отзыв уже оставлен");

        var review = new Review
        {
            User = executor,
            Description = reviewCreationDto.ReviewText,
            Order = order,
            Rating = 5,
            Created = DateTime.Now.ToUniversalTime()
        };
        return await reviewRepository.CreateReviewAsync(review);
    }


    public async Task<User> GetCustomerUserByOrderId(int orderId)
    {
        var order = await orderRepository.GetOrderByIdAsync(orderId);
        return await userRepository.GetUserByIdAsync(order.Creator.Id);
    }

    public async Task<DetailOrderDto> UpdateOrder(User creatorUser, int orderId, OrderCreationDto order)
    {
        var updatedOrder = await orderRepository.GetOrderByIdAsync(orderId);
        if (updatedOrder.Creator.Id != creatorUser.Id)
            throw new ArgumentException("Нет прав для удаления заказа");
        if (updatedOrder.IsCompleted)
            throw new ArgumentException("Заказ уже завершен, его нельзя изменить");

        var skills = new List<Skill>(order.OrderSkills.Count);
        foreach (var skill in order.OrderSkills)
            skills.Add(await competenciesRepository.GetSkillByNameAsync(skill.Name));

        var categories = new List<Category>(order.OrderCategories!.Count);
        foreach (var category in order.OrderCategories)
            categories.Add(await competenciesRepository.GetCategoryByNameAsync(category.Name));

        var orderRoles = await
            orderRoleRepository.GetOrderRolesByNames(order.OrderVacancies.Select(x => x.OrderRole.Name).ToList());
        updatedOrder.Price = order.Price;
        updatedOrder.Name = order.Name;
        updatedOrder.Description = order.Description;
        updatedOrder.Deadline = order.Deadline;
        updatedOrder.OrderVacancies = order.OrderVacancies.Select(dto => new OrderVacancy
        {
            OrderRole = orderRoles.First(x => x.Name.Equals(dto.OrderRole.Name)),
            MaxWorkers = dto.MaxWorkers,
            OrderId = updatedOrder.Id
        }).ToList();
        updatedOrder.OrderCategories = categories;
        updatedOrder.OrderSkills = skills;
        await orderRepository.SaveChangesAsync(updatedOrder);
        return new DetailOrderDto(updatedOrder, creatorUser);
    }

    public async Task RemoveResponse(Executor userExecutor, int orderId)
    {
        var response = await responseRepository.GetUserOrderResponseAsync(userExecutor.Id, orderId);
        if (response == null)
            throw new ArgumentException("Не существует заявки");

        if (response.IsAccept != null)
            throw new ArgumentException("На вашу заявку уже ответили");
        await responseRepository.RemoveResponseAsync(response);
    }

    public async Task DeleteExecutorFromOrder(DeleteExecutorDto deleteExecutorDto)
    {
        var response =
            await responseRepository.GetUserOrderResponseAsync(deleteExecutorDto.ExecutorId, deleteExecutorDto.OrderId);
        if (response == null)
            throw new ArgumentException("Пользователь не выполняет данный заказ");
        var order = await orderRepository.GetOrderByIdAsync(deleteExecutorDto.OrderId);
        //var executor = await userRepository.GetUserByExecutorIdAsync(deleteExecutorDto.ExecutorId);
        if (order.IsCompleted)
            throw new ArgumentException("Заказ уже завершен, вы не можете его изменить");
        if (response.IsCompleted)
            throw new ArgumentException("Исполнитель уже завершил заказ, вы не можете его удалить");
        await responseRepository.RemoveResponseAsync(response);
    }

    public async Task FinishProject(User creatorId, int orderId)
    {
        var order = await orderRepository.GetOrderByIdAsync(orderId);
        if (order.Creator.Id != creatorId.Id)
            throw new ArgumentException("Вы не можете завершить чужой заказ");
        order.IsCompleted = true;
        await orderRepository.SaveChangesAsync(order);

        //throw new ArgumentException("Вы не можете завершить чужой заказ");
    }
}