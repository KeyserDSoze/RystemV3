namespace System
{
    public sealed record TryResponse<T>(T? Entity, Exception? Exception = null)
    {
        public static implicit operator T(TryResponse<T> response)
            => response.Entity!;
    }
}
