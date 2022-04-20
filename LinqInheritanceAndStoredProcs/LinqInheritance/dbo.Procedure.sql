CREATE PROCEDURE [dbo].[CreateContractEmployee]
	@Name nvarchar(50),
	@Gender int,
	@HourlyPay int,
	@HoursWorked int	
AS
	INSERT INTO Employee(
		Name,Gender,Discriminator
	) values(
		@Name,@Gender,2
	)

	INSERT INTO Contract(
		EmployeeID,HourlyPay,HoursWorked
	)values(
		SCOPE_IDENTITY(),@HourlyPay,@HoursWorked
	)
RETURN 0
