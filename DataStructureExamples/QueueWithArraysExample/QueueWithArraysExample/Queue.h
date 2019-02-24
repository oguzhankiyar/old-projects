#pragma once
class Queue
{
public:
	Queue(int size = 10);
	~Queue(void);
	bool isEmpty(void);
	bool isFull(void);
	bool Enqueue(const double data);
	bool Dequeue();
	void DisplayQueue();
private:
	int front;
	int rear;
	int counter;
	int maxSize;
	double* values;
};

