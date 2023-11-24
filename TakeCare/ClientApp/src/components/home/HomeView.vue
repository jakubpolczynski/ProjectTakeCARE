<template>
  <div>
    <div class="row">
      <h1 class="col-10 d-flex justify-content-start">
        Welcome to TakeCare – Your Personal Health Ally!
      </h1>
      <span class="col-2 d-flex justify-content-end">Today is {{ date }}</span>
    </div>
    <div class="pt-4">
      <span>
        <p>
          At TakeCare, we understand that your health is your top priority, and finding the right
          doctor should be a seamless experience. Our platform connects you with experienced
          healthcare professionals who are dedicated to providing personalized care tailored to your
          unique needs.
        </p>
        <p>
          Here, you're more than just a patient; you're a part of the TakeCare family. Our doctors
          are not only experts in their fields but also compassionate individuals who listen
          attentively and respond with the utmost care.
        </p>
        <p>Experience a new era of healthcare:</p>
        <ul>
          <li><b>Easy Appointments:</b> Schedule and manage your appointments effortlessly.</li>
          <li>
            <b>Comprehensive Care:</b> From general health concerns to specialized treatments, our
            doctors cover it all.
          </li>
          <li>
            <b>Virtual Consultations:</b> Consult with top doctors from the comfort of your home.
          </li>
          <li>
            <b>Health Records:</b> Access your medical history and health data securely anytime,
            anywhere.
          </li>
          <li>
            <b>Trusted Advice:</b> Receive reliable medical advice backed by the latest research and
            best practices.
          </li>
          <p>
            Join us on a journey towards a healthier, happier you. Your well-being is our mission.
            Welcome to TakeCare – where your health is in caring hands.
          </p>
          <b>Start your health journey with us today!</b>
        </ul>
        <p></p>
      </span>
    </div>
  </div>
</template>
<script setup lang="ts">
const date = new Date().toISOString().slice(0, 10);
</script>
