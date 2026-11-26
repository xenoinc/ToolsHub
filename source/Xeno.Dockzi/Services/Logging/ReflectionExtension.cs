namespace Xeno.Dockzi.Services.Logging;

using System;
using System.Linq;
using System.Reflection;

public static class ReflectionExtension
{
  public static TAttr GetFirstCustomAttribute<TAttr>(this Type type)
      where TAttr : Attribute
  {
    return Attribute.GetCustomAttributes(type, typeof(TAttr)).FirstOrDefault() as TAttr;
  }

  public static TAttr GetFirstCustomAttribute<TAttr>(this PropertyInfo info)
      where TAttr : Attribute
  {
    return Attribute.GetCustomAttributes(info, typeof(TAttr)).FirstOrDefault() as TAttr;
  }

  public static TAttr GetFirstCustomAttribute<TAttr>(this Assembly assembly)
      where TAttr : Attribute
  {
    return Attribute.GetCustomAttributes(assembly, typeof(TAttr)).FirstOrDefault() as TAttr;
  }
}
