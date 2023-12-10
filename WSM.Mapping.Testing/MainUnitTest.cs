using Xunit;
using WSM.Mapping; // Assuming the mapping logic is implemented in this namespace

namespace WSM.Mapping.Tests;

public class MainUnitTest
{
    [Fact]
    public void TestMapping_WithAddress2()
    {
        // Arrange
        var customer = new Customer
        {
            Name = "Wisam Idris",
            Address = new Address
            {
                Line1 = "123"
            },
            Address2 = new Address
            {
                Line1 = "321"
            }
        };

        // Act
        var customerDto = WSMMapping.Map<CustomerDto>(customer);

        // Assert
        Assert.NotNull(customerDto);
        Assert.Equal(customer.Name, customerDto.Name);
        Assert.NotNull(customerDto.Address);
        Assert.Equal(customer.Address.Line1, customerDto.Address.Line1);
        Assert.NotNull(customerDto.Address2);
        Assert.Equal(customer.Address2.Line1, customerDto.Address2.Line1);
    }

    //[Fact]
    //public void TestMapping_WithoutAddress2()
    //{
    //    // Arrange
    //    var customerDto = new CustomerDto
    //    {
    //        Name = "John Doe",
    //        Address = new AddressDto
    //        {
    //            Line1 = "456"
    //        },
    //        Address2 = null
    //    };

    //    // Act
    //    var customer = WSMMapping.Map<Customer, CustomerDto>(customerDto);

    //    // Assert
    //    Assert.NotNull(customer);
    //    Assert.Equal(customerDto.Name, customer.Name);
    //    Assert.NotNull(customer.Address);
    //    Assert.Equal(customerDto.Address.Line1, customer.Address.Line1);
    //    Assert.Null(customer.Address2);
    //}

    //[Fact]
    //public void TestMapping_NullDto()
    //{
    //    // Arrange
    //    CustomerDto customerDto = null;

    //    // Act
    //    var customer = WSMMapping.Map<Customer, CustomerDto>(customerDto);

    //    // Assert
    //    Assert.Null(customer);
    //}
}
public class Customer
{
    public string Name { get; set; }
    public Address Address { get; set; }
    public Address Address2 { get; set; }
}
public class Address
{
    public string Line1 { get; set; }
}
public class CustomerDto
{
    public string Name { get; set; }
    public AddressDto Address { get; set; }
    public Address Address2 { get; set; }
}
public class AddressDto
{
    public string Line1 { get; set; }
}