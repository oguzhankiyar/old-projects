#define trigPin 13
#define echoPin 12

#define motor1_geri 4
#define motor1_ileri 5
#define motor2_geri 6
#define motor2_ileri 7

void setup() {
  Serial.begin (9600);
  pinMode(trigPin, OUTPUT);
  pinMode(echoPin, INPUT);
  
  pinMode(motor1_geri, OUTPUT);
  pinMode(motor1_ileri, OUTPUT);
  pinMode(motor2_geri, OUTPUT);
  pinMode(motor2_ileri, OUTPUT);

  pinMode(2, OUTPUT);
}

void loop() {
  digitalWrite(2, HIGH);
  long duration, distance;
  digitalWrite(trigPin, LOW);  // Added this line
  delayMicroseconds(2); // Added this line
  digitalWrite(trigPin, HIGH);

  delayMicroseconds(10); // Added this line
  digitalWrite(trigPin, LOW);
  duration = pulseIn(echoPin, HIGH);
  distance = (duration/2) / 29.1;
  
  if (distance <= 15){
    digitalWrite(motor1_geri, LOW);
    digitalWrite(motor1_ileri, LOW);
    digitalWrite(motor2_geri, LOW);
    digitalWrite(motor2_ileri, LOW);
    Serial.println("durdu");
  }
  else {
    digitalWrite(motor1_geri, LOW);
    digitalWrite(motor1_ileri, HIGH);
    digitalWrite(motor2_geri, LOW);
    digitalWrite(motor2_ileri, HIGH);
    Serial.print(distance);
    Serial.println(" cm");
  }
  delay(500);
}
