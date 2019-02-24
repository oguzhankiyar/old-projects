
#define SOL_GERI 9
#define SAG_GERI 11
#define SOL_ILERI 10
#define SAG_ILERI 8

#define TRIG A0
#define ECHO A1

int count = 0;
char myChar = ' ';
String myString = "";
int isForward = 0;

void setup() {
  pinMode(SOL_GERI, OUTPUT);
  pinMode(SAG_GERI, OUTPUT);
  pinMode(SOL_ILERI, OUTPUT);
  pinMode(SAG_ILERI, OUTPUT);
  
  pinMode(TRIG, OUTPUT);
  pinMode(ECHO, INPUT);
  
  Serial.begin(9600);
}
void loop() {

  long duration, distance;
  digitalWrite(TRIG, LOW);  // Added this line
  delayMicroseconds(2); // Added this line
  digitalWrite(TRIG, HIGH);
  delayMicroseconds(10); // Added this line
  digitalWrite(TRIG, LOW);
  duration = pulseIn(ECHO, HIGH);
  distance = (duration/2) / 29.1;
  
  if (distance <= 20 && isForward == 1) {
    digitalWrite(SOL_GERI, LOW);
    digitalWrite(SOL_ILERI, LOW);
    digitalWrite(SAG_GERI, LOW);
    digitalWrite(SAG_ILERI, LOW);
  }
  
  if (Serial.available()) {
    myChar = (char)Serial.read();
    count++;
    delay(100);

    myString += myChar;
    myChar = ' ';
    if (count % 2 == 0) {
      isForward = 0;
      if (myString.equalsIgnoreCase("FR")) {
        digitalWrite(SOL_GERI, LOW);
        digitalWrite(SOL_ILERI, HIGH);
        digitalWrite(SAG_GERI, LOW);
        digitalWrite(SAG_ILERI, HIGH);
        isForward = 1;
      }
      else if (myString.equalsIgnoreCase("BK")) {
        digitalWrite(SOL_GERI, HIGH);
        digitalWrite(SOL_ILERI, LOW);
        digitalWrite(SAG_GERI, HIGH);
        digitalWrite(SAG_ILERI, LOW);
      }
      else if (myString.equalsIgnoreCase("LF")) {
        digitalWrite(SOL_ILERI, HIGH);
        digitalWrite(SOL_GERI, LOW);
      }
      else if (myString.equalsIgnoreCase("LB")) {
        digitalWrite(SOL_GERI, HIGH);
        digitalWrite(SOL_ILERI, LOW);
      }
      else if (myString.equalsIgnoreCase("LS")) {
        digitalWrite(SOL_GERI, LOW);
        digitalWrite(SOL_ILERI, LOW);
      }
      else if (myString.equalsIgnoreCase("RF")) {
        digitalWrite(SAG_ILERI, HIGH);
        digitalWrite(SAG_GERI, LOW);
      }
      else if (myString.equalsIgnoreCase("RB")) {
        digitalWrite(SAG_GERI, HIGH);
        digitalWrite(SAG_ILERI, LOW);
      }
      else if (myString.equalsIgnoreCase("RS")) {
        digitalWrite(SAG_GERI, LOW);
        digitalWrite(SAG_ILERI, LOW);
      }
      else if (myString.equalsIgnoreCase("ST")) {
        digitalWrite(SOL_GERI, LOW);
        digitalWrite(SOL_ILERI, LOW);
        digitalWrite(SAG_GERI, LOW);
        digitalWrite(SAG_ILERI, LOW);
      }
      myString = "";
    }
    delay(100);
  }
}
