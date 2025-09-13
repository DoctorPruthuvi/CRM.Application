using CRM.Domain.Entities;
using CRM.Domain.Interfaces;
using CRM.Application.Interfaces;

namespace CRM.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            return await _customerRepository.GetAllAsync();
        }

        public async Task<Customer?> GetCustomerByIdAsync(int id)
        {
            return await _customerRepository.GetByIdAsync(id);
        }

        public async Task<Customer> CreateCustomerAsync(Customer customer)
        {
            if (await _customerRepository.EmailExistsAsync(customer.Email))
            {
                throw new InvalidOperationException("Email already exists.");
            }

            return await _customerRepository.AddAsync(customer);
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            if (await _customerRepository.EmailExistsAsync(customer.Email, customer.AccountId))
            {
                throw new InvalidOperationException("Email already exists.");
            }

            var existingCustomer = await _customerRepository.GetByIdAsync(customer.AccountId);
            if (existingCustomer == null)
            {
                throw new InvalidOperationException("Customer not found.");
            }

            // Update properties of the tracked entity
            existingCustomer.FirstName = customer.FirstName;
            existingCustomer.LastName = customer.LastName;
            existingCustomer.Email = customer.Email;
            existingCustomer.PhoneNumber = customer.PhoneNumber;
            existingCustomer.Address = customer.Address;
            existingCustomer.City = customer.City;
            existingCustomer.State = customer.State;
            existingCustomer.Country = customer.Country;

            await _customerRepository.UpdateAsync(existingCustomer);
        }

        public async Task DeleteCustomerAsync(int id)
        {
            await _customerRepository.DeleteAsync(id);
        }
    }
}