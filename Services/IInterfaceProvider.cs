namespace Services;

public interface IInterfaceProvider
{
    T GetService<T>();
    void AddStaticService<T, U>();
    void AddScopedService<T, U>();
    void AddTransientService<T, U>();
}