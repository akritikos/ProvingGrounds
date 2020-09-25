namespace Kritikos.Configuration.Persistence.Abstractions
{
	/// <summary>
	/// Exposes barebones auditing functionality on a multi-user system.
	/// </summary>
	/// <typeparam name="T">Type of auditor identifying field.</typeparam>
	public interface IAuditable<T>
	{
		T CreatedBy { get; set; }

		T UpdatedBy { get; set; }
	}
}
