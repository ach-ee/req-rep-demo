package main

import (
	"fmt"

	zmq "github.com/pebbe/zmq4"
)

func main() {
	identity := "go-client"
	responder, _ := zmq.NewSocket(zmq.DEALER)
	responder.SetIdentity(identity)
	defer responder.Close()

	responder.Connect("tcp://localhost:5555")

	for {
		request, _ := responder.RecvMessage(0)
		fmt.Printf("Recevied: [%s]\n", request[1])

		responder.Send(identity, zmq.SNDMORE)
		responder.Send("", zmq.SNDMORE)
		responder.Send("Go Response", 0)
	}
}
