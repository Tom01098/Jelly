Jelly has a minimal syntax focusing on code clarity. Whitespace is insignificant but newlines are as only one statement is allowed per line.

## Literals
Jelly only has one literal type - a double-precision floating point number (eg: `4`, `5.2`, or `-2.34`).

## Variables
### Initialisation
Similar to most C-like languages, a variable initialisation has a variable name, an equals symbol, and then an expression which evaluates to a value.

```
variable = 3 + 6
```

### Mutating
Variables mutations are structurally similar to their initialisation, but they use '=>' rather than '='.

```
variable => variable - 1
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

## Operators
> Operations are currently parsed right-to-left as it was the easiest way to parse it. If precedence is needed, use parenthesis to explicitly determine the order of operations.

| Operator | Use                                                                        |
|----------|----------------------------------------------------------------------------|
| +        | Add the LHS and RHS                                                        |
| -        | Subtract the RHS from the LHS, or the negative value of the following term |
| *        | Multiply the LHS and RHS                                                   |
| /        | Divide the LHS by the RHS                                                  |
| %        | The remainder after dividing the LHS from the RHS                          |
| ==       | 1 if the LHS and RHS are equal, 0 otherwise                                |
| !=       | 0 if the LHS and RHS are equal, 1 otherwise                                |
| <<       | 1 if the LHS is less than the RHS, 0 otherwise                             |
| >>       | 1 if the LHS is greater than the RHS, 0 otherwise                          |
| <=       | 1 if the LHS is less than or equal to the RHS, 0 otherwise                 |
| >=       | 1 if the LHS is greater than or equal to the RHS, 0 otherwise              |
| \|       | The absolute value of the term inside (eg: \|-4\|)                         |

## Conditions
A condition is an expression which is used in a block to determine whether to run it or not.
A zero value is equivalent to false, everything else is considered true.
The unary operator `!` is used before a condition to 'flip' it; if it is true, it will become false, and vice-versa.

## Blocks
### If-Elif-Else
The keywords `if`, `elif`, and `else` are used to start a conditional block. 
`if` and `elif` blocks have a condition after the keyword, if it is true, it will execute that block.
Each block ends with the `end` keyword.

```
if x == 2
	@ If x is 2
end
elif x == 4
	@ If x is 4
end
elif x == 5
	@ If x is 5
end
else
	@ If x is not 2, 4, or 5
end
```

### Loop
A loop is declared with the `loop` keyword, and is followed by a condition.
While the condition evaluates to true, the loop body will execute over and over again.

```
loop x >> 100
	WriteLine<x>
	x => RandomIntBetween<0, 1000>
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
