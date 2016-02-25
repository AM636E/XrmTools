## XrmTools

### Tools for simplifying Microsoft Dynamics Crm api buiding.
XrmTools simplifies most commom tasks that developer performs why building Web apis with Dynamics Crm backend.
Such as :
* Creating an organization service ( with of without impersonation )
* Executing queries ( with ColumnSet generated automatically based on mapping )
* Manipulating entities
* Entity to Model object mapping.
* Logging and exception handling for WebApi.
### Setting up

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