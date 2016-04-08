## XrmTools
[![License](http://img.shields.io/badge/License-MIT-blue.svg)](http://opensource.org/licenses/MIT)
[![Build status](https://ci.appveyor.com/api/projects/status/6oqa59nqotrm0xay/branch/master?svg=true)](https://ci.appveyor.com/project/AM636E/xrmtools/branch/master)

### Tools for simplifying Microsoft Dynamics CRM REST API buiding.
XrmTools simplifies most commom tasks that developer performs when building Web APIs with Dynamics CRM backend.
Such as :
* Creating an organization service ( with of without impersonation )
* Executing queries ( with ColumnSet generated automatically based on mapping )
* Manipulating entities
* Entity to Model object mapping.
* Logging and exception handling for WebApi.
* Configuration providers and encryption.
* Web requests, supporting sandboxed environments.

## Packages:
##### DynamicaLabs.Tools:
  Configuration, Logging, Encryption
 [![NuGet version](https://badge.fury.io/nu/DynamicaLabs.Tools.svg)](https://badge.fury.io/nu/DynamicaLabs.Tools)
##### DynamicaLabs.CrmTools
  Sandboxed web request, Crm logging( with destination in crm entity )
   [![NuGet version](https://badge.fury.io/nu/DynamicaLabs.CrmTools.svg)](https://badge.fury.io/nu/DynamicaLabs.CrmTools)
##### DynamicaLabs.WebTools
  Handlers and Filters for web api for logging, and exception handling.
   [![NuGet version](https://badge.fury.io/nu/DynamicaLabs.WebTools.svg)](https://badge.fury.io/nu/DynamicaLabs.WebTools)
##### DynamicaLabs.XrmTools.Construction
   Model construction from crm entities. [![NuGet version](https://badge.fury.io/nu/DynamicaLabs.XrmTools.Construction.svg)](https://badge.fury.io/nu/DynamicaLabs.XrmTools.Construction)
##### DynamicaLabs.XrmTools.Data
  Contains IOrganizationServiceWrapper that allows easelly convert OrganizationService query results to model entities, perform impopersonated queries. [![NuGet version](https://badge.fury.io/nu/DynamicaLabs.XrmTools.Data.svg)](https://badge.fury.io/nu/DynamicaLabs.XrmTools.Data)


### Examples

Suppose you have this class in your code.
```csharp
// Map crm entity with model entity.
[CrmEntity("account")]
class Account 
{
    // Map accountid field with this property.
    [CrmField("accountid")]
    public Guid Id {get; set;}
    
    // If crm field is entity reference we can map particular entity reference field
    // Such as name or id with no NullReferenceException 
    [CrmField("primarycontactid", FieldHandler = typeof(GuidEntityReferenceFieldHandler))]
    public Guid PrimaryContactId { get; set; }
    
    [CrmField("primarycontactid", FieldHandler = typeof(NameEnRefFieldHandler)]
    public Name PrimaryContactName { get; set; }
    
    [CrmField("name")]
    public string Name { get; set; }
}
```
To retrieve a list of crm accounts and map them to this entity we need to do this:
```csharp
// Setting up the repository.
IEntityRepository er = new DefaultEntityRepository(new ConfigXrmConnectionStringProvider(connectionSettings), new ReflectionEntityConstructory());

// accounts will have IEnumerable<Account> type.
var accounts = er.GetEntities<Account>(new QueryExpression("account"));
foreach(var account in accounts) {
  Console.WriteLine(account.Name);
  Console.WriteLine(account.PrimaryContactName);
}
```
To retrieve an antities based on some simple criteria
```csharp
var accounts = er.QueryByAttributes<Account>("account", 
new Dictionary<string, object>
{
  ["name"] = "ven"
}, onlyActive: true, strict: false);
```

To create crm entity from model entity
```csharp
var acc = new Account { "Name" = "dnl" };
var created = acc.Create(acc);
Console.WriteLine(created.Id);
```
If you created an entity in crm and want to check if all fields are properly mapped
```csharp
var account = new Account{"Name" = "valia"};
account.Id = er.Create(account).Id;
TestUtils.AssertEqual(account, "account", account.Id);
```
