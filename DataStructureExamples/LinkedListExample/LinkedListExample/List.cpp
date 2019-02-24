#include "List.h"

using namespace std;

List::List(void)
{
	head = NULL;
}
List::~List(void)
{
	Node* curNode = head;
	Node* nextNode = NULL;
	while (curNode)
	{
		nextNode = curNode -> nextNode;
		delete curNode;
		curNode = nextNode;
	}
}
bool List::isEmpty()
{
	return head == NULL;
}
int List::findNode(double data)
{
	Node* curNode = head;
	int curIndex = 1;
	while (curNode && curNode -> data != data)
	{
		curNode = curNode -> nextNode;
		curIndex++;
	}
	if (curNode)
		return curIndex;
	return 0;
}
Node* List::insertNode(int index, double data)
{
	if (index < 0)
		return NULL;
	Node* curNode = head;
	int curIndex = 1;
	while (curNode && curIndex < index)
	{
		curNode = curNode -> nextNode;
		curIndex++;
	}
	if (!curNode && index > 0)
		return NULL;
	Node* newNode = new Node();
	newNode -> data = data;
	if (index == 0)
	{
		newNode -> nextNode = head;
		head = newNode;
	}
	else
	{
		newNode -> nextNode = curNode -> nextNode;
		curNode -> nextNode = newNode;
	}
	return newNode;
}
int List::deleteNode(double data)
{
	Node* curNode = head;
	Node* prevNode = NULL;
	int curIndex = 1;
	while (curNode && curNode -> data != data)
	{
		prevNode = curNode;
		curNode = curNode -> nextNode;
		curIndex++;
	}
	if (curNode)
	{
		if (prevNode)
			prevNode -> nextNode = curNode ->nextNode;
		else
			head = curNode -> nextNode;
		delete curNode;
		return curIndex;
	}
	return 0;
}
void List::displayList()
{
	Node* curNode = head;
	int nodeCount = 0;
	while (curNode)
	{
		cout << curNode -> data << endl;
		curNode = curNode -> nextNode;
		nodeCount++;
	}
	cout << "Total " << nodeCount << " node(s)" << endl;
}