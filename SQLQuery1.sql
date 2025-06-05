alter procedure spSearchEmployees
  @FirstName VARCHAR(100) = NULL,
    @LastName VARCHAR(100) = NULL,
    @Gender VARCHAR(50) = NULL,
    @Salary INT = NULL
as
begin
select * from Employees 
where 
	(FirstName = @FirstName or @FirstName is null) and
	(LastName = @LastName or @LastName is null) and
	(Gender = @Gender or @Gender is null) and
	(Salary = @Salary or @Salary is null)
end
go

insert into Employees values
('Mark', 'Hastings', 'Male', 60000),
('Steve', 'Pound', 'Male', 45000),
('Den', 'Hosloms', 'Male', 7000),
('Philip', 'Hastings', 'Male', 45000),
('Mary', 'Lambeth', 'Female', 30000),
('Valarie', 'Vikings', 'Female', 35000),
('John', 'Stanmore', 'Male', 80000)

alter table Employees 
add id int primary key identity