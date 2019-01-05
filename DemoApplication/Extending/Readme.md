# Extending Chain of Responsibility
This is an demo of extending the exaple from **Chain of Responsibility 1**.

Let's say somebody whats to add:
1. A class that can sum up the total aproved by each person and the whole company
1. A manual approve option with acception / reject. This gives these states:
   1. Post expense
   2. Approval pending
   3. Approval accepted
   4. Approval rejected

The trick is, don't want to change too much of our existing code.
