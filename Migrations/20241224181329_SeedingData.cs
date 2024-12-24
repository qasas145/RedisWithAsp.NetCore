using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace my_redis.Migrations
{
    /// <inheritdoc />
    public partial class SeedingData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData("Drivers", new string[]{"Id", "Name", "DriverNumber"}, new object[] {1, "Muhammad", 20});
            migrationBuilder.InsertData("Drivers", new string[]{"Id", "Name", "DriverNumber"}, new object[] {2, "Ahmed", 30});
            migrationBuilder.InsertData("Drivers", new string[]{"Id", "Name", "DriverNumber"}, new object[] {3, "Qasas", 50});
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("delete from drivers");
        }
    }
}
