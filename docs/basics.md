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
`TODO`

## Comments
An '@' symbol denotes that the rest of the line will not be processed by the compiler and can be used to comment.

## Statement Continuation
A semicolon at the end of a line denotes that the statement continues on to the next line.

```
x = y * ;
    y
```

is exactly the same as

```
xSquared = x * x
```

This is useful in cases where lines might be very long and it would be more readable to write it over several.
