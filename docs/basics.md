*Syntax is subject to change*

Jelly has a minimal syntax focusing on code clarity. Whitespace is insignificant but newlines are as only one statement is allowed per line.

## Literals
Jelly only has one literal type - a double-precision floating point number.

## Variables
### Initialisation
Similar to most C-like languages, a variable initialisation has a variable name, an equals symbol, and then an expression which evaluates to a value.

```
variable = 3 + 6
```

### Mutating
Variables mutations are structurally similar to their initialisation, but they use '<=' rather than '='.

```
variable <= variable - 1
```

## Functions
A function is declared with the function name and then the parameters in angled brackets, separated by commas. The function ends with the 'end' keyword. A tilde '~' indicates the result of the following expression will be returned.

```
Square<x>
    ~x * x
end
```

Omitting the expression after the tilde will return NaN. If a method ends without returning it will also return NaN.

```
Divide<top, bottom>
    if bottom == 0
        ~
    end
    
    ~top / bottom
end
```

## Comments
An '@' symbol denotes that the rest of the line will not be processed by the compiler and can be used to comment.

## Statement Continuation
A semicolon at the end of a line denotes that the statement continues on to the next line.

```
xSquared = x * ;
           x
```

is exactly the same as

```
xSquared = x * x
```

This is useful in cases where lines might be very long and it would be more readable to write it over several.
