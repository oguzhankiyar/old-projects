#include <iostream>
#include "Stack.h"

using namespace std;

void main()
{
	cout << "### Stack with Linked Lists Example ###" << endl;
	Stack stack;
	stack.Push(1);
	stack.Push(2);
	stack.Push(3);
	stack.Push(4);
	stack.Push(5);
	stack.DisplayStack();
	stack.Pop();
	stack.Pop();
	stack.DisplayStack();
	getchar();
}
