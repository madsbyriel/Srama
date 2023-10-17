namespace Services;

public interface IInterfaceProvider
{
    T GetService<T>();
}