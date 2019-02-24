#include <iostream>
#include "Queue.h"

using namespace std;

void main()
{
	cout << "Queue with Arrays Example" << endl;
	Queue queue;
	queue.Enqueue(1);
	queue.Enqueue(2);
	queue.Enqueue(3);
	queue.Enqueue(4);
	queue.Enqueue(5);
	queue.DisplayQueue();
	queue.Dequeue();
	queue.Dequeue();
	queue.DisplayQueue();
	getchar();
}
