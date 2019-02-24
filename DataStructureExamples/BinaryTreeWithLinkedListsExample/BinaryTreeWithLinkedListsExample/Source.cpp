#include <iostream>
#include "List.h"

using namespace std;

void main()
{
	cout << "### Binary Tree with Linked Lists Example ###" << endl;
	List list;
	list.addNode(5);
	list.addNode(4);
	list.addNode(6);
	list.addNode(3);
	list.addNode(15);
	list.listNode(list.head);
	list.addNode(15);
	if (list.findNode(15))
		cout << "the 15 element found." << endl;
	if (!list.findNode(7))
		cout << "the 7 element doesn't found." << endl;
	cout << "min element of the tree: " << list.findMin() -> data << endl;
	cout << "max element of the tree: " << list.findMax() -> data << endl;
	getchar();
}
