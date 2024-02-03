# Joaat Brute Forcer
- Brute Forcer for JOAAT hashes

# Dictionaries
- This brute forcer uses "dictionaries" to brute force hashes.
- Your dictionaries should be .txt files that are filled with words or whatever you want.

# How to use the brute force format
- This is in a similar format to the "Custom" option in the "Output" group.
- Dictionaries that you want to use must be encased in curly brackets {} with the file name of the dictionary.
- File names of dictionaries must start with "list." for it to be recognized by the program.
- Example w/ dictionary named "list.strings.txt":  **Some{strings}String**
- A maximum of 10 dictionaries can be used in the format at a time.
- Dictionaries also comes with 3 formatting options that can be applied at the end of a dictionary name:
	- "**\*U**" To make all letters uppercase
	- "**\*L**" To make all letters lowercase
	- "**\*F**" To make only the first letter lowercase
	- Example w/ dictionary named "list.strings.txt":  **Some{strings\*U}String**
- By default, the string case of the dictionary will be whatever it is in the file if no custom format option was put on it.

# How the brute forcer works
- The brute forcer gets ALL possible string combinations from the dictionaries you put in the format
	- This is why the more dictionaries you add, the slower it gets. Smaller dictionaries will allow you to add more in the format
- Here's what it does internally. Example format:  **{strings1}_{strings2}**
```
first_first
first_second
first_third
second_first
second_second
second_third
third_first
third_second
third_third

etc
```

# Hash List Info
- The hash list accepts both hexadecimal and decimal numbers.
- It will trim all trailing and leading whitespace before starting a brute force.
- There is a regular expression that is used to find hexadecimal inputs, meaning you don't have to remove any non hexadecimal characters before putting them in

# String Hash Mode
- Allows you to generate the JOAAT hash of strings
- Just start inputting your strings in the hash list text box

# Tip for faster brute forcing
- Create smaller dictionaries with specific words that might be in the hashes you are brute forcing
