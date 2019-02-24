#include "List.h"

class Stack : private List
{
public:
	Stack(void);
	~Stack(void);
	double Top();
	void Push(const double data);
	double Pop();
	void DisplayStack();
};

