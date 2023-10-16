using System.Reflection;

namespace Services;

public class InterfaceProvider : IInterfaceProvider
{
    private static Dictionary<Type, ConstructorInfo> staticInterfaceToConstructor = new Dictionary<Type, ConstructorInfo>();
    private static Dictionary<Type, ConstructorInfo> scopedInterfaceToConstructor = new Dictionary<Type, ConstructorInfo>();    
    private static Dictionary<Type, ConstructorInfo> transientInterfaceToConstructor = new Dictionary<Type, ConstructorInfo>();

    private static Dictionary<Type, object> staticObjects = new Dictionary<Type, object>();
    private Dictionary<Type, object> scopedObjects = new Dictionary<Type, object>();
    private Dictionary<Type, object> transientObjects = new Dictionary<Type, object>();
    

    // Notes: No matter the services, we first need to find a constructor first. We find the constructor by 
    // looking for interfaces in the dictionaries. The very first match will do. We add that to the constructor maps, along with the interface.
    public void AddStaticService<T, U>() 
    {
        Type interfaze = typeof(T);
        Type clazz = typeof(U);
        ThrowIfUnassignable(interfaze, clazz);

        ConstructorInfo constructor = FindConstructorOrThrow(clazz, staticInterfaceToConstructor);
        staticInterfaceToConstructor[interfaze] = constructor;

        // Only in the static services is it allowed to immediately instantiate an object.
        T? newObject = (T?)InvokeConstructorFromMaps(constructor, staticObjects) ?? throw new Exception("Failed when constructing an instance of " + clazz.Name);
        staticObjects[interfaze] = newObject;
    }

    public void AddScopedService<T, U>()
    {
        throw new NotImplementedException();
    }

    public void AddTransientService<T, U>()
    {
        throw new NotImplementedException();
    }

    private static void ThrowIfUnassignable(Type interfaze, Type clazz) 
    {
        if (!interfaze.IsAssignableFrom(clazz)) 
        {
            throw new Exception(interfaze.Name + " is not assignable from type " + clazz.Name);
        }
    }

    private ConstructorInfo FindConstructorOrThrow(Type clazz, params Dictionary<Type, ConstructorInfo>[] interfacesToConstructors) 
    {
        ConstructorInfo[] constructors = clazz.GetConstructors();
        foreach (ConstructorInfo constructor in constructors) 
        {
            bool constructorFound = true;
            ParameterInfo[] parameters = constructor.GetParameters();
            foreach (ParameterInfo parameter in parameters) 
            {
                if (!InterfaceContainedIn(parameter.ParameterType, interfacesToConstructors)) 
                {
                    constructorFound = false;
                    break;
                }
            }

            if (constructorFound) return constructor; 
        }

        throw new Exception("Couldn't find constructor for " + clazz.Name);
    }

    private bool InterfaceContainedIn(Type interfaze, params Dictionary<Type, ConstructorInfo>[] interfacesToConstructors)
    {
        foreach (Dictionary<Type, ConstructorInfo> map in interfacesToConstructors) 
        {
            if (map.ContainsKey(interfaze)) return true;
        }
        return false;
    }

    private object? InvokeConstructorFromMaps(ConstructorInfo constructor, params Dictionary<Type, object>[] interfacesToObjects) 
    {
        ParameterInfo[] parameters = constructor.GetParameters();
        object[] paramValues = new object[parameters.Length];
        for (int i = 0; i < parameters.Length; i++) 
        {
            ParameterInfo parameter = parameters[i];
            Type paramType = parameter.ParameterType;
            object paramValue;
            foreach (Dictionary<Type, object> mapToObjects in interfacesToObjects) 
            {
                if (mapToObjects.TryGetValue(paramType, out paramValue!)) // ! tells the out paramValue that it definitely isn't null when exciting.
                {
                    paramValues[i] = paramValue;
                }
            }
        }

        return constructor.Invoke(paramValues);
    }

    public T GetService<T>()
    {
        Type serviceType = typeof(T);
        T obj;
        if ((obj = (T)staticObjects[serviceType]) != null) return obj;
        if ((obj = (T)scopedObjects[serviceType]) != null) return obj; 
        if ((obj = (T)transientObjects[serviceType]) != null) return obj; 
        throw new Exception("No service exists with interface " + serviceType.Name);
    }
}