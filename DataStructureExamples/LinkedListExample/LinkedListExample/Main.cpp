#include <iostream>
#include "List.h"

using namespace std;

void main()
{
	cout << "### Linked List Example ###" << endl;
	List list;
	list.insertNode(0, 5);
	list.insertNode(1, 10);
	list.insertNode(2, 15);
	list.insertNode(3, 20);
	list.insertNode(4, 25);
	list.displayList();
	list.deleteNode(15);
	list.displayList();
	getchar();
}