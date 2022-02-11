const int JOYSTICK_BUTTON_PIN =  2;
const int          X_AXIS_PIN = A1;
const int          Y_AXIS_PIN = A0;

void setup() {
  Serial.begin( 9600 );
  pinMode(14, INPUT);
  pinMode(15, INPUT);
  pinMode(16, INPUT);
}

void loop() {
  int inputX = analogRead(X_AXIS_PIN);
  int inputY = analogRead(Y_AXIS_PIN);
  bool bValue = digitalRead(JOYSTICK_BUTTON_PIN);
  Serial.print(inputX, DEC);
  Serial.print(",");
  Serial.print(inputY, DEC);
  Serial.print(",");
  Serial.print(bValue);
  Serial.print("\n");
  delay(15);
}
