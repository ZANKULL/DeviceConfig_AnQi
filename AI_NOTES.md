# Prompts

1. Troubleshoot why I cannot create a new connection in SQL Server Management Studio
2. Create a function that can print out data in a table where the rows of data align with the row below it
3. How to fill a datatable and then get a reader from the same sql afterwards
4. How to trigger the program by using DeviceCfg.exe in the command-line
5. Fix an error where doing string concatenation on a null value will trigger an error
6. Fix errors where the program could not run by feeding in code and error messages

# Print a table in console

I initially got AI to help print out the table in console but the results were not aligned at all. The AI just added a fixed padding to everything and gave me suggestions on how to do it so I just did it myself.

# Fix
I found the greatest width of each of the columns using a DataTable and stored it inside a list. Then I used a Reader to print out each data in each row and do calculations using the list to determine the spacing.


