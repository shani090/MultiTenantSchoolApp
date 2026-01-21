namespace SmartEduHub.Interface
{
    public interface ITenantContextAccessor
    {
        string? GetTenantId();
        void SetTenantId(string tenantId);
    }
}
