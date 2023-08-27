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

    public OrderService(IOrderRepository orderRepository, ICompetenciesRepository competenciesRepository,
        IUserRepository userRepository, IReviewRepository reviewRepository, IResponseRepository responseRepository)
    {
        this.orderRepository = orderRepository;
        this.competenciesRepository = competenciesRepository;
        this.userRepository = userRepository;
        this.reviewRepository = reviewRepository;
        this.responseRepository = responseRepository;
    }

    public async Task<Order> CreateOrder(Customer customer, OrderCreationDto creationDto)
    {
        var skills = new List<Skill>(creationDto.OrderSkills!.Count);
        foreach (var skill in creationDto.OrderSkills)
            skills.Add(await competenciesRepository.GetSkillByNameAsync(skill.Name));

        var categories = new List<Category>(creationDto.OrderCategories!.Count);
        foreach (var category in creationDto.OrderCategories)
            categories.Add(await competenciesRepository.GetCategoryByNameAsync(category.Name));

        var order = new Order
        {
            Price = creationDto.Price,
            Name = creationDto.Name,
            Description = creationDto.Description,
            MaxWorkers = creationDto.MaxWorkers,
            Deadline = creationDto.Deadline,
            CompanyName = customer.CompanyName,
            Customer = customer,
            OrderSkills = skills,
            OrderCategories = categories,
            IsPublished = true,
            IsCompleted = false
        };
        return await orderRepository.CreateOrderAsync(order);
    }

    public async Task PublishOrder(Customer customer, int orderId)
    {
        var order = await orderRepository.GetOrderByIdAsync(orderId);

        if (!customer.Orders!.Contains(order))
            throw new ArgumentException("Заказ не принадлежит пользователю");
        order.IsPublished = true;

        await orderRepository.SaveChangesAsync(order);
    }

    public async Task DeleteOrder(Customer customer, int orderId)
    {
        var order = await orderRepository.GetOrderByIdAsync(orderId);

        if (!customer.Orders!.Contains(order))
            throw new ArgumentException("Заказ не принадлежит пользователю");

        if (order.Responses!.Any(response => response.IsAccept == true) || order.IsCompleted)
            throw new ArgumentException("Невозможно удалить, заказ уже приняли");

        await orderRepository.DeleteOrderAsync(order);
    }

    public async Task CreateResponse(Executor executor, int orderId)
    {
        var order = await orderRepository.GetOrderByIdAsync(orderId);

        if (order.IsCompleted)
            throw new ArgumentException("Невозможно откликнуться, набор исполнителей завершен");

        await orderRepository.CreateResponseAsync(executor, order);
    }

    public async Task UpdateResponse(bool isAccept, int responseId)
    {
        var response = await orderRepository.GetResponseById(responseId);
        var order = await orderRepository.GetOrderByIdAsync(response.Order.OrderId);

        if (order.IsCompleted ||
            (order.MaxWorkers > 0 && order.MaxWorkers == order.Responses!.Count(r => r.IsAccept == true)))
            throw new ArgumentException("Невозможно принять исполнителя, набор исполнителей завершен");

        response.IsAccept = isAccept;
        await orderRepository.UpdateResponseAsync(response);
    }

    public async Task<List<CustomerCommonOrderDto>> GetCustomerOrders(User user, int offset, int limit)
    {
        var orders = await orderRepository
            .GetCustomerOrdersAsync(user.Customer!, offset, limit);
        return orders.Select(order => new CustomerCommonOrderDto(order.OrderId, order.Price, order.Name,
            order.Description,
            order.MaxWorkers, order.CompanyName,
            order.Responses!.Count(r => r.IsAccept == null), order.Deadline,
            false,
            new OrderSkillsDto(order), new OrderCategoriesDto(order), order.IsCompleted)).ToList();
    }

    public async Task<List<CommonOrderDto>> GetExecutorOrders(User user, int offset, int limit)
    {
        var orders = await orderRepository
            .GetExecutorOrdersAsync(user.Executor!, offset, limit);
        return orders.Select(order => new CommonOrderDto(order.OrderId, order.Price, order.Name, order.Description,
            order.MaxWorkers, order.CompanyName,
            order.Responses!.Count(r => r.IsAccept == null), order.Deadline,
            true,
            order.Responses.First(response => response.ExecutorId == user.Executor.ExecutorId).IsCompleted,
            new OrderSkillsDto(order), new OrderCategoriesDto(order))).ToList();
    }

    public async Task<DetailOrderDto> GetDetailOrderInfo(User customer, int orderId)
    {
        var order = await orderRepository.GetOrderByIdAsync(orderId);

        var result = new DetailOrderDto(order, customer);

        var ids = (order.Responses ?? new List<Response>(0)).Where(response => response.IsAccept != false)
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
        List<SkillDto>? skillDtos = null, List<CategoryDto>? categoryDtos = null)
    {
        var dbOrders = await orderRepository.GetActualOrdersAsync();
        var dbSkills = dbOrders.SelectMany(order => order.OrderSkills!).ToList();
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
                .SelectMany(s => s.Orders);

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
                var response = responseRepository.GetUserOrderResponseAsync(user.Executor.ExecutorId, order.OrderId)
                    .Result;
                isResponsed = response != null;
                if (response != null)
                    isCompleted = response.IsCompleted;
            }

            return new CommonOrderDto(
                order.OrderId,
                order.Price,
                order.Name,
                order.Description,
                order.MaxWorkers,
                order.CompanyName,
                order.Responses.Count,
                order.Deadline,
                isResponsed,
                isCompleted,
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
        if (user != null && user.Executor != null)
        {
            var response = order.Responses!.FirstOrDefault(response =>
                response.ExecutorId == user.Executor.ExecutorId);

            isResponsed = response != null;
            isAccepted = response?.IsAccept ?? false;
            isCompleted = response?.IsCompleted ?? false;
        }

        var customer = await userRepository.GetUserByIdAsync(order.Customer.UserId);
        return new CommonOrderInfoDto
        {
            OrderId = orderId,
            Price = order.Price,
            Name = order.Name,
            Description = order.Description,
            IsResponded = isResponsed,
            IsAccepted = isAccepted,
            CompanyName = order.CompanyName,
            Deadline = order.Deadline,
            IsCompleted = isCompleted,
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
            Nickname = customer.Username,
            Address = customer.Customer.Address,
            INN = customer.Customer.INN,
            Email = customer.Email,
            Phone = customer.Phone,
            Contacts = new ContactsUpdateDto
            {
                Messager = customer.UserContacts == null ? "" : customer.UserContacts.Messager,
                VkNickname = customer.UserContacts == null ? "" : customer.UserContacts.VkNickname
            }
        };
    }

    public async Task<Review> CreateReview(ReviewCreationDto reviewCreationDto)
    {
        var executor = await userRepository.GetUserByExecutorIdAsync(reviewCreationDto.ExecutorId);
        var order = await orderRepository.GetOrderByIdAsync(reviewCreationDto.OrderId);
        var response = order.Responses.FirstOrDefault(r => r.ExecutorId == reviewCreationDto.ExecutorId);
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
        return await userRepository.GetUserByCustomerIdAsync(order.Customer.CustomerId);
    }

    public async Task<DetailOrderDto> UpdateOrder(Customer userCustomer, int orderId, OrderCreationDto order)
    {
        var updatedOrder = await orderRepository.GetOrderByIdAsync(orderId);
        if (updatedOrder.Customer.CustomerId != userCustomer.CustomerId)
            throw new ArgumentException("Нет прав для удаления заказа");
        if (updatedOrder.IsCompleted)
            throw new ArgumentException("Заказ уже завершен, его нельзя изменить");

        var skills = new List<Skill>(order.OrderSkills!.Count);
        foreach (var skill in order.OrderSkills)
            skills.Add(await competenciesRepository.GetSkillByNameAsync(skill.Name));

        var categories = new List<Category>(order.OrderCategories!.Count);
        foreach (var category in order.OrderCategories)
            categories.Add(await competenciesRepository.GetCategoryByNameAsync(category.Name));


        updatedOrder.Price = order.Price;
        updatedOrder.Name = order.Name;
        updatedOrder.Description = order.Description;
        updatedOrder.MaxWorkers = order.MaxWorkers;
        updatedOrder.Deadline = order.Deadline;
        updatedOrder.OrderSkills = skills;
        updatedOrder.OrderCategories = categories;
        await orderRepository.SaveChangesAsync(updatedOrder);
        return new DetailOrderDto(updatedOrder, userCustomer.User);
    }

    public async Task RemoveResponse(Executor userExecutor, int orderId)
    {
        var response = await responseRepository.GetUserOrderResponseAsync(userExecutor.ExecutorId, orderId);
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
        var executor = await userRepository.GetUserByExecutorIdAsync(deleteExecutorDto.ExecutorId);
        if (order.IsCompleted)
            throw new ArgumentException("Заказ уже завершен, вы не можете его изменить");
        if (response.IsCompleted)
            throw new ArgumentException("Исполнитель уже завершил заказ, вы не можете его удалить");
        await responseRepository.RemoveResponseAsync(response);
    }

    public async Task FinishProject(Customer customer, int orderId)
    {
        var order = await orderRepository.GetOrderByIdAsync(orderId);
        if (order.Customer.CustomerId != customer.CustomerId)
            throw new ArgumentException("Вы не можете завершить чужой заказ");
        order.IsCompleted = true;
        await orderRepository.SaveChangesAsync(order);

        //throw new ArgumentException("Вы не можете завершить чужой заказ");
    }
}