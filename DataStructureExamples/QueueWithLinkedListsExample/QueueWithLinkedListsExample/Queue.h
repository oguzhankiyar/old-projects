#include <iostream>
#include "Node.h"

using namespace std;

class Queue {
public:
	Queue(void);
	~Queue(void);
	bool IsEmpty();
	bool Enqueue(double x);
	bool Dequeue();
	void DisplayQueue(void);
private:
	Node* front;
	Node* rear;
	int counter;
};

