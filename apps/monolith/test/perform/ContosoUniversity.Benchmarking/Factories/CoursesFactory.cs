namespace ContosoUniversity.Benchmarking.Factories;

using Domain.Course;

internal static class CoursesFactory
{
    public static Course CreateCourse(int iteration)
        => Course.Create(
            1234,
            $"Some course, iteration: {iteration}",
            iteration % (Credits.MaxValue + 1),
            IdsFactory.SelectDepartmentIdForIteration(iteration));
}
