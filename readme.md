# JSON Pointer for C&#35;

This is an implementation of [JSON Pointer](http://tools.ietf.org/html/draft-ietf-appsawg-json-pointer-08).

This project is a derived work from:
- [Tavis.JsonPointer](https://github.com/tavis-software/Tavis.JsonPointer) another C# implementation, but without type casting or set capability
- [jsonpointer](https://github.com/janl/node-jsonpointer) an example node implementation

## Usage
```csharp
var obj = @"{ foo: 1, bar: { baz: 2}, qux: [3, 4, 5]}";

JsonPointer.Get<int>(obj, "/foo");     // returns 1
JsonPointer.Get<int>(obj, "/bar/baz"); // returns 2
JsonPointer.Get<int>(obj, "/bar/baz"); // returns 2
JsonPointer.Get<int>(obj, "/qux/0");   // returns 3
JsonPointer.Get<int>(obj, "/qux/1");   // returns 4
JsonPointer.Get<int>(obj, "/qux/2");   // returns 5
JsonPointer.Get<int>(obj, "/quo");     // throws ArgumentException

// if you want to test if JSON pointer path is valid
int result;
if(JsonPointer.TryGet<int>(obj, "/quo", out result))
{
	// only if result was found
}

obj = JsonPointer.Set(obj, "/foo", 6);   // sets obj.foo = 6;
obj = JsonPointer.Set(obj, "/qux/-", 6); // sets obj.qux = [3, 4, 5, 6]
```
### Another example:
```csharp
public class Book
{
    public string Title { get; set; }
    public string Author { get; set; }
}

var sample = @"{
  'books': [
    {
      'title' : 'The Great Gatsby',
      'author' : 'F. Scott Fitzgerald'
    },
    {
      'title' : 'The Grapes of Wrath',
      'author' : 'John Steinbeck'
    }
  ]
}";

var author = JsonPointer.Get<string>(sample, "/books/1/author"); // returns "John Steinbeck"
var book = JsonPointer.Get<string>(sample, "/books/1");          // returns second book (zero indexed array)
var collection = JsonPointer.Get<List<Book>>(sample, "/books");  // returns List<Book> with two items

// adds another book to end of collection
sample = JsonPointer.Set(sample, "/books/-", new Book { Title = "Jayne Eyre", Author = "Charlotte Brontë" });
```

## Author

(c) 2016 - Jacob Bair <jacob.bair@shavlik.com>

## License

MIT License.            