namespace SqlMaker.Common
{
    public delegate void DataUpdatingHandler<T>(object sender, DataBaseUpdatingEventArg<T> e);
    public delegate void DataUpdatedHandler<T>(object sender, DataBaseUpdatedEventArg<T> e);
}
