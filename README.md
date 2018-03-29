# Dapper.Futher

This is Base [Dapper](https://github.com/StackExchange/Dapper) and [Dapper.Contrib](https://github.com/StackExchange/Dapper/tree/master/Dapper.Contrib)

This library just provide specific columns update.

## Table 
```
[Dapper.Contrib.Extensions.Table("az_tbl_test")]
public class TblTest
{
	public static string Name = "az_tbl_test";

	[Dapper.Contrib.Extensions.ExplicitKey]
	public int a { get; set; }

	[Dapper.Contrib.Extensions.ExplicitKey]
	public int b { get; set; }

	public string str { get; set; }
}
```  

## Sample

```
Dapper.Contrib.Extensions.SqlMapperExtensions.Insert(conn, new Table.TblTest() { a = 10, b = 20 });
Dapper.Contrib.Extensions.SqlMapperExtensions.Insert(conn, new Table.TblTest() { a = 30, b = 40 });
Dapper.Contrib.Extensions.SqlMapperExtensions.Insert(conn, new Table.TblTest() { a = 50, b = 60 });

Table.TblTest tmp = new Table.TblTest();
tmp.a = 10;
tmp.b = 200;

Dapper.Contrib.Extensions.SqlMapperExtensions.Update(conn, tmp);  // <- do not update because key is multiple.

Dapper.Dm.Extensions.SqlMapperExtensions.UpdateFields<Table.TblTest>(conn, 
  new { a = 10, b = 200, str="str" }, 
  new { a = 50 });
```
