using OutsourcePlatformApp.Dto;
using OutsourcePlatformApp.Models;
using OutsourcePlatformApp.Repository;

namespace OutsourcePlatformApp.Service;

public class ExecutorService
{
    private readonly IExecutorRepository executorRepository;
    private readonly IOrderRepository orderRepository;
    private readonly IUserRepository userRepository;
    private readonly IReviewRepository reviewRepository;

    public ExecutorService(IExecutorRepository executorRepository, IOrderRepository orderRepository,
        IUserRepository userRepository, IReviewRepository reviewRepository)
    {
        this.executorRepository = executorRepository;
        this.orderRepository = orderRepository;
        this.userRepository = userRepository;
        this.reviewRepository = reviewRepository;
    }

    public async Task<List<Executor>> GetExecutorsWithChosenFiltersAsync(
        int offset, int limit,
        List<SkillDto>? skillDtos = null, List<CategoryDto>? categoryDtos = null)
    {
        var dbExecutors = await executorRepository.GetExecutorListAsync();
        var dbSkills = dbExecutors.SelectMany(executor => executor.Skills!).ToList();
        var dbCategories = dbExecutors.SelectMany(executor => executor.Categories!).ToList();

        if (skillDtos == null)
        {
            var intersectedCategoryNames = dbCategories
                .Select(x => x.Name)
                .Intersect(categoryDtos!.Select(x => x.Name));
            var executorCategories = dbCategories
                .Where(x => intersectedCategoryNames.Contains(x.Name))
                .SelectMany(c => c.Executors);

            return executorCategories
                .Distinct()
                .Skip(offset)
                .Take(limit)
                .ToList();
        }

        if (categoryDtos == null)
        {
            var intersectedSkillNames = dbSkills
                .Select(x => x.Name)
                .Intersect(skillDtos.Select(x => x.Name));

            var executorSkills = dbSkills
                .Where(x => intersectedSkillNames.Contains(x.Name))
                .SelectMany(c => c.Executors);

            return executorSkills
                .Distinct()
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
            .SelectMany(s => s.Executors).Distinct().ToList();

        var categories = dbCategories
            .Where(x => intersectCategoryNames.Contains(x.Name))
            .SelectMany(c => c.Executors).Distinct().ToList();

        return skills.Union(categories)
            .Distinct()
            .Skip(offset)
            .Take(limit)
            .ToList();
    }

    public async Task<ExecutorListResponse> GetExecutorList(int offset, int limit,
        List<SkillDto>? skillDtos = null, List<CategoryDto>? categoryDtos = null)
    {
        var executors = new List<Executor>();
        if (skillDtos == null && categoryDtos == null)
        {
            executors = await executorRepository.GetExecutorsAsync(offset, limit);
        }
        else
        {
            executors = await GetExecutorsWithChosenFiltersAsync(offset, limit, skillDtos, categoryDtos);
        }

        var count = executorRepository.GetTotalExecutorsCountAsync();
        return new ExecutorListResponse(executors.Select(executor => new CommonExecutorDto(
                executor.Id,
                executor.User.Username,
                executor.User.Name,
                executor.User.Surname,
                executor.User.SecondName,
                executor.User.Reviews!.Count,
                new ExecutorSkillsDto(executor),
                new ExecutorCategoriesDto(executor))
            ).ToList(),
            await count);
    }

    public async Task FinishProject(Executor executor, int orderId, bool isFinish)
    {
        var order = await orderRepository.GetOrderByIdAsync(orderId);
        var userResponse = order.OrderVacancies.SelectMany(x => x.Responses.Where(x => x.ExecutorId == executor.Id))
            .FirstOrDefault(response =>
                response.ExecutorId == executor.Id && response.IsAccept == true);
        if (userResponse == null)
            throw new ArgumentException("Пользователь не записан на проект");
        if (order.IsCompleted)
            throw new ArgumentException(
                "Сожалеем, но заказчик уже завершил проект. Не переживайте, Ваша работа была проделана не зря, ведь вы всегда можете загрузить его в портфолио :)");

        if (isFinish)
        {
            userResponse.IsCompleted = true;
        }
        else
        {
            userResponse.IsCompleted = false;
            userResponse.IsResourceUploaded = false;
        }

        await orderRepository.SaveChangesAsync(order);
    }

    public async Task<ExecutorDetailCloseDto> GetDetailCloseInfo(int executorId)
    {
        var finishProjectCount = await executorRepository.GetExecutorFinishProjects(executorId);
        var user = await userRepository.GetUserByExecutorIdAsync(executorId);
        return new ExecutorDetailCloseDto(user, finishProjectCount);
    }

    public async Task<ExecutorDetailOpenDto> GetDetailOpenInfo(int executorId)
    {
        var finishProjectCount = await executorRepository.GetExecutorFinishProjects(executorId);
        var user = await userRepository.GetUserByExecutorIdAsync(executorId);
        return new ExecutorDetailOpenDto(user, finishProjectCount);
    }

    public async Task<bool> IsExecutorConnectedToCustomer(Customer customer, int executorId)
    {
        return await executorRepository.IsExecutorConnectWithCustomer(customer, executorId);
    }

    public async Task<List<ReviewDto>> GetExecutorReviews(int executorId, int offset, int limit)
    {
        var reviews = await reviewRepository.GetReviewsByExecutorId(executorId, offset, limit);
        return reviews.Select(review => new ReviewDto
        {
            Id = review.Id,
            Description = review.Description,
            OrderId = review.Order.Id,
            ProjectnName = review.Order.Name,
            CompanyName = review.Order.CompanyName,
            ProjectCompleted = review.Created,
            Competitences = review.Order.OrderCategories
                .Select(c => c.Name)
                .ToList()
        }).ToList();
    }

    public async Task<List<ReviewDto>> GetExecutorReviews(User user, int orderId)
    {
        var reviews = await reviewRepository.GetReviewsByExecutorId(user.Executor.Id, 0, int.MaxValue);
        return reviews.Where(x => x.Order.Id == orderId).Select(review => new ReviewDto
        {
            Id = review.Id,
            Description = review.Description,
            OrderId = review.Order.Id,
            ProjectnName = review.Order.Name,
            CompanyName = review.Order.CompanyName,
            ProjectCompleted = review.Created,
            Competitences = review.Order.OrderCategories
                .Select(c => c.Name)
                .ToList()
        }).ToList();
    }

    public async Task<Executor> GetExecutorById(int executorId)
    {
        return await executorRepository.GetExecutorById(executorId);
    }

    public async Task SetFileToResponse(Executor executor, int orderId)
    {
        var order = await orderRepository.GetOrderByIdAsync(orderId);
        var response = order.OrderVacancies.SelectMany(x => x.Responses)
            .FirstOrDefault(x => x.ExecutorId == executor.Id);
        response.IsResourceUploaded = true;
        await orderRepository.SaveChangesAsync(order);
    }
}