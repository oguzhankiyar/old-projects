#include <SoftwareSerial.h>
 
SoftwareSerial mySerial(0, 1); // RX, TX
int led = A1;
 
void setup()
{
  pinMode(led, OUTPUT);
  mySerial.begin(9600);
  mySerial.println("LED uygulamasi");
}
 
void loop()
{
  char ch = mySerial.read();
  mySerial.println(ch);
  if (ch == '1')
  {
    digitalWrite(led, HIGH);
    mySerial.println("LED yandi");
  }
  if (ch == '0')
  {
    digitalWrite(led, LOW);
    mySerial.println("LED sondu");
  }
}
