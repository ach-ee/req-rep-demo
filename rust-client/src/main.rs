fn main() {
    let context = zmq::Context::new();
    let responder = context.socket(zmq::DEALER).unwrap();
    let identity = "rust-client";

    responder.set_identity(identity.as_bytes()).unwrap();
    assert!(responder.connect("tcp://localhost:5555").is_ok());

    loop {
        responder.recv_bytes(0).unwrap();
        let message = responder.recv_string(0).unwrap().unwrap();
        println!("Received {}", message);

        responder.send(identity, zmq::SNDMORE).unwrap();
        responder.send("", zmq::SNDMORE).unwrap();
        responder.send("Rust Response", 0).unwrap();
    }
}
