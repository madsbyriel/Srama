using Services;

IProjectInfo info = new ProjectInfo();
ILogging logging = new Logging(info);

try
{
    throw new Exception("Testing stuff");
}
catch (System.Exception e)
{
    logging.LogException(e);
}