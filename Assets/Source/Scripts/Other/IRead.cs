public interface IRead<T>
{
    public T Read();
}

public interface IReadFromArray<T>
{
    public T Read(int index);
}
