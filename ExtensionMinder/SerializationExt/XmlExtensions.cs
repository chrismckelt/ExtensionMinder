namespace ExtensionMinder.SerializationExt
{
  public static class XmlExtensions
  {
    public static int ToXmlBoolean(this bool value)
    {
      return value ? 1 : 0;
    }
  }
}