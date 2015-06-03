Cecho is an extended version of windows' echo command
using:

  * color escape codes
  * a few csharp escape codes
> and
  * supporting some ANSI escape codes.

## Usage ##
```
cecho "text"
```
### Escape codes ###

  * `\##` - # is an hex digit [0-F] representing a color, 1st is the backcolor
  * `\m` - Reset color
  * `\n` - Line-feed
  * `\r` - Carriage-return
  * `\t` - (Horizontal-) Tab
  * `\u####` - A unicode character. #### - the hexadecimal value. (must be 4 digits)
  * `\U########` - A UTF32 character. ######## - the hexadecimal value.
  * `\x1b[##m` - ANSI color syntax - ## as specified in the table below
  * `\x1b[0m` - Reset color");
  * `\"` - The character "
  * `\\` - The character \

### Color Table ###
```
                 Regular                                Ansi
    Black        0    DarkGray     8  |  Black        30    Gray         30;1
    DarkBlue     1    Blue         9  |  DarkRed      31    Red          31;1
    DarkGreen    2    Green        A  |  DarkGreen    32    Green        32;1
    DarkCyan     3    Cyan         B  |  DarkYellow   33    Yellow       33;1
    DarkRed      4    Red          C  |  DarkBlue     34    Blue         34;1
    DarkMagenta  5    Magenta      D  |  DarkMagenta  35    Magenta      35;1
    DarkYellow   6    Yellow       E  |  DarkCyan     36    Cyan         36;1
    Gray         7    White        F  |  DarkGray     37    White        37;1
```
## Example ##

```
@echo off
cecho ""
cecho "\40   \m\50    \m\40   \m\30 \m\30 \m\20   \m"
cecho "\40 \m\50 \m\40 \m\30 \m\30 \m\20 \m\20 \m"
cecho "\40 \m\50    \m\40 \m\30    \m\20 \m\20 \m"
cecho "\40 \m\50 \m\40 \m\30 \m\30 \m\20 \m\20 \m"
cecho "\40   \m\50    \m\40   \m\30 \m\30 \m\20   \m by elisherer"
cecho ""
```

Outputs:

<img src='http://cecho.googlecode.com/svn/trunk/cecho/screenshot.jpg' border='0' />