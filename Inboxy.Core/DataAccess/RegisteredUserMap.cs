namespace Inboxy.Core.DataAccess
{
	using Inboxy.Core.Domain;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Metadata.Builders;

	public class RegisteredUserMap : IEntityTypeConfiguration<RegisteredUser>
	{
		public void Configure(EntityTypeBuilder<RegisteredUser> entity)
		{
			entity.ToTable("AspNetUsers");
			entity.HasKey(c => c.Id);

			entity.Property(t => t.Id).HasColumnName("Id");
			entity.Property(t => t.Name).HasColumnName("UserName");
		}
	}
}