#include <iostream>
#include "Stack.h"

using namespace std;

Stack::Stack(void)
{
}
Stack::~Stack(void)
{
}
double Stack::Top()
{
	if (isEmpty())
		cout << "Error: the stack is empty." << endl;
	else
		return head -> data;
}
void Stack::Push(const double data)
{
	insertNode(0, data);
}
double Stack::Pop()
{
	if (isEmpty())
		cout << "Error: the stack is empty." << endl;
	else
	{
		double val = head -> data;
		deleteNode(val);
		return val;
	}
}
void Stack::DisplayStack()
{
	displayList();
}
