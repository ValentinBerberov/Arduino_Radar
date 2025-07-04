// C++ code
//
#include <math.h>
#include <Servo.h>

const int trigPin = 4;
const int echoPin = 2;

Servo myservo;

int pos = 0;
unsigned long duration = 0;
int distance = 0;

String serial_out;

void setup()
{
  myservo.attach(7); 
  myservo.write(0);

  pinMode(trigPin, OUTPUT);
  pinMode(echoPin, INPUT);
  
  Serial.begin(9600);
}

void loop()
{

  for (pos = 0; pos <= 160; pos += 1) { 
    myservo.write(pos); 

    digitalWrite(trigPin, LOW);
    delayMicroseconds(2);
    digitalWrite(trigPin, HIGH);
    delayMicroseconds(10);
    digitalWrite(trigPin, LOW);

    duration = pulseIn(echoPin, HIGH);
    distance = (duration*.0343)/2;

    Serial.print(pos);
    Serial.print(" ");
    Serial.println(distance);

    delay(25);
  }

  delay(1000);

  for (pos = 160; pos >= 0; pos -= 1) { 
    myservo.write(pos);

    digitalWrite(trigPin, LOW);
    delayMicroseconds(2);
    digitalWrite(trigPin, HIGH);
    delayMicroseconds(10);
    digitalWrite(trigPin, LOW);

    duration = pulseIn(echoPin, HIGH);
    distance = (int)(duration*.0343)/2;

    Serial.print(pos);
    Serial.print(" ");
    Serial.println(distance);

    delay(25);
  }
  delay(1000);
}
