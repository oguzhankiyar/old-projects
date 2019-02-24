#include <iostream>
#include "Queue.h"

using namespace std;

Queue::Queue(void)
{
	front = rear = NULL;
	counter = 0;
}
Queue::~Queue(void)
{
}
bool Queue::IsEmpty()
{
	if (counter) return false;
	return true;
}
bool Queue::Enqueue(double data)
{
	Node* newNode = new Node();
	newNode -> data = data;
	newNode -> nextNode = NULL;
	if (IsEmpty())
		front = rear = newNode;
	else
	{
		rear -> nextNode = newNode;
		rear = newNode;
	}
	counter++;
	return true;
}
bool Queue::Dequeue()
{
	if (IsEmpty())
	{
		cout << "Error: the queue is empty." << endl;
		return false;
	}
	else
	{
		Node* tempNode = front -> nextNode;
		delete front;
		front = tempNode;
		counter--;
	}
}
void Queue::DisplayQueue()
{
	Node* curNode = front;
	while (curNode)
	{
		cout << curNode -> data << endl;
		curNode = curNode -> nextNode;
	}
	if (IsEmpty())
		cout <<	"the queue is empty" << endl;
	else
		cout << "the queue has " << counter << " element(s)." << endl;
}
