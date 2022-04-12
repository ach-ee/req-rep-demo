package main

import (
	"fmt"
	"time"

	zmq "github.com/pebbe/zmq4"
)

func main() {
	identity := "go-client"
	responder, _ := zmq.NewSocket(zmq.DEALER)
	responder.SetIdentity(identity)
	defer responder.Close()

	responder.Connect("tcp://localhost:5555")

	poller := zmq.NewPoller()
	poller.Add(responder, zmq.POLLIN)

	for {
		sockets, _ := poller.Poll(time.Second)

		if len(sockets) > 0 {
			request, _ := responder.RecvMessage(0)
			fmt.Printf("Recevied: [%s]\n", request[1])

			responder.Send(identity, zmq.SNDMORE)
			responder.Send("", zmq.SNDMORE)
			responder.Send("Go Response", 0)
		} else {
			responder.Send(identity, zmq.SNDMORE)
			responder.Send("", zmq.SNDMORE)
			responder.Send("Go Request", 0)
		}
	}
}
