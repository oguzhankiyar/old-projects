#include <iostream>
#include "Queue.h"

using namespace std;

Queue::Queue(int size)
{
	values = new double[size];
	maxSize = size - 1;
	counter = 0;
	front = 0;
	rear = -1;
}
Queue::~Queue(void)
{
	delete[] values;
}
bool Queue::isEmpty()
{
	return !counter;
}
bool Queue::isFull()
{
	return counter == maxSize;
}
bool Queue::Enqueue(double data)
{
	if (isFull())
	{
		cout << "Error: the queue is full." << endl;
		return false;
	}
	else
	{
		rear = (rear + 1) % maxSize;
		values[rear] = data;
		counter++;
		return true;
	}
}
bool Queue::Dequeue()
{
	if (isEmpty())
	{
		cout << "Error: the queue is empty." << endl;
		return false;
	}
	else
	{
		front = (front + 1) % maxSize;
		counter--;
		return true;
	}
}
void Queue::DisplayQueue()
{
	for (int i = 0; i < counter; i++)
	{
		cout << values[front + i] << endl;
	}
	if (isEmpty())
		cout << "the queue is empty" << endl;
	else
		cout << "the queue has " << counter << " element(s)" << endl;
}
