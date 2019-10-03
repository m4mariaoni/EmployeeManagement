using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class MockEmployeeRepository : IEmployeeRepository
    {
        private List<Employee> _employeeList;
        public MockEmployeeRepository()
        {
            _employeeList = new List<Employee>()
            {
               new Employee(){ Id =1, Name = "Mary", Department=Dept.HR, Email ="mary@pagram.com"},
                 new Employee(){ Id =2, Name = "John", Department=Dept.IT, Email ="johny@pagram.com"},
                   new Employee(){ Id =3, Name = "Sam", Department=Dept.IT, Email ="sam@pagram.com"}
            };
            
        }

        public Employee Add(Employee employee)
        {
            employee.Id = _employeeList.Max(e => e.Id) + 1;
            _employeeList.Add(employee);
            return employee;
        }

        public Employee Delete(int id)
        {
           var employee =  _employeeList.FirstOrDefault(e => e.Id == id);
            if(employee != null)
            {
                _employeeList.Remove(employee);
            }
            return employee;
        }



        public IEnumerable<Employee> GetAllEmployees()
        {
            return _employeeList;
        }

        public Employee GetEmployee(int Id)
        {
            return _employeeList.FirstOrDefault(e => e.Id == Id);
        }

        public Employee Update(Employee employeeUpdate)
        {
            var employee = _employeeList.FirstOrDefault(e => e.Id == employeeUpdate.Id);
            if (employee != null)
            {
                employee.Department = employeeUpdate.Department;
                employee.Email = employeeUpdate.Email;
                employee.Name = employeeUpdate.Name;
               
            }
            return employee;
        }
    }
}
