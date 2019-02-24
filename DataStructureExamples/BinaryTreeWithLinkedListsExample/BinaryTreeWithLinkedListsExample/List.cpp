#include "List.h"

List::List(void)
{
	head = NULL;
}
List::~List(void)
{
}
void List::addNode(int data, Node* kok)
{
	if (kok == NULL)
		kok = head;
	Node* newNode = new Node(data);
	if (head == NULL)
	{
		head = newNode;
	}
	else
	{
		if (data < kok -> data)
		{
			if (kok -> leftNode == NULL)
				kok -> leftNode = newNode;
			else
				addNode(data, kok -> leftNode);
		}
		else if (data > kok -> data)
		{
			if (kok -> rightNode == NULL)
				kok -> rightNode = newNode;
			else
				addNode(data, kok -> rightNode);
		}
		else
			cout << "Error: the tree has " << data << " element." << endl;
	}
}
bool List::editNode(int oldData, int newData)
{
	if (deleteNode(oldData))
	{
		addNode(newData);
		return true;
	}
	return false;
}
bool List::deleteNode(int data)
{
	Node* parentNode = NULL;
	Node* curNode = head;

	// find parent of node and current node
	while (parentNode != NULL && parentNode -> data != data)
	{
		curNode = parentNode;
		if (data < parentNode -> data)
			parentNode = parentNode -> leftNode;
		else
			parentNode = parentNode -> rightNode;
	}

	// if tree doesn't have the node, return false
	if (curNode == NULL)
		return false;
	
	// if node has two children, change left max node with current node
	if (curNode -> leftNode != NULL && curNode -> rightNode != NULL)
	{
		Node* maxOfNode = findMax(curNode -> leftNode);
		maxOfNode -> leftNode = curNode -> leftNode;
		curNode = maxOfNode;
	}

	Node* qc;
	// node has one child any more
	if (curNode -> leftNode != NULL)
		qc = curNode -> leftNode;
	else
		qc = curNode -> rightNode;


	return true;
}
void List::listNode(Node* kok)
{
	if (kok)
	{
		listNode(kok -> leftNode);
		cout << kok -> data << endl;
		listNode(kok -> rightNode);
	}
}
Node* List::findNode(int data)
{
	Node* curNode = head;
	while (curNode != NULL && curNode -> data != data)
	{
		if (data > curNode -> data)
			curNode = curNode -> rightNode;
		else
			curNode = curNode -> leftNode;
	}
	return curNode;
}
Node* List::findMin(Node* kok)
{
	Node* curNode = kok == NULL ? head : kok;
	while (curNode != NULL && curNode -> leftNode != NULL)
	{
		curNode = curNode -> leftNode;
	}
	return curNode;
}
Node* List::findMax(Node* kok)
{
	Node* curNode = kok == NULL ? head : kok;
	while (curNode != NULL && curNode -> rightNode != NULL)
	{
		curNode = curNode -> rightNode;
	}
	return curNode;
}