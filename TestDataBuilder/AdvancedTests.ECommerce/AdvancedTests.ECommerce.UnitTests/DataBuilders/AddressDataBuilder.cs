using AdvancedTests.ECommerce.Domain;
using Bogus;

namespace AdvancedTests.ECommerce.UnitTests.DataBuilders;

public class AddressDataBuilder
{
    private string _street;
    private string _city;
    private string _state;
    private string _zipCode;
    private int _number;

    public AddressDataBuilder()
    {
        var faker = new Faker("pt_BR");
        _street = faker.Address.StreetAddress();
        _city = faker.Address.City();
        _state = faker.Address.StateAbbr();
        _zipCode = faker.Address.ZipCode();
        _number = faker.Random.Int(1, 1000);
    }

    public AddressDataBuilder WithStreet(string street)
    {
        _street = street;
        return this;
    }

    public AddressDataBuilder WithCity(string city)
    {
        _city = city;
        return this;
    }

    public AddressDataBuilder WithState(string state)
    {
        _state = state;
        return this;
    }

    public AddressDataBuilder WithZipCode(string zipCode)
    {
        _zipCode = zipCode;
        return this;
    }
    
    public AddressDataBuilder WithNumber(int number)
    {
        _number = number;
        return this;
    }

    public Address Build()
    {
        return new Address(_street, _city, _state, _zipCode, _number);
    }
}